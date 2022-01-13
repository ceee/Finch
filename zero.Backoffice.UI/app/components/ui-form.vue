<template>
  <form class="ui-form" @keydown="onKeydown" @submit.prevent="onSubmit" @change="onChange">
    <slot v-if="loadingState === 'default'" v-bind="slotProps" />
    <div v-if="loadingState == 'loading'" class="ui-form-loading">
      <i class="ui-form-loading-progress"></i>
    </div>
    <div v-if="loadingState === 'error'">
      error [not implemented]
    </div>
    <!--<form-error-view v-if="loadingState === 'error'" :error="loadingError" />-->
  </form>
</template>


<script lang="ts">
  import { defineComponent } from 'vue';
  //import FormErrorView from './form-error-view.vue';
  import * as overlays from '../services/overlay';
  import * as notifications from '../services/notification';
  import { arrayGroupBy } from '../utils/arrays';

  export default defineComponent({
    name: 'uiForm',

    //components: { FormErrorView },

    props: {
      errorComponents: {
        type: Array,
        default: () => ['uiError']
      },
      inputComponents: {
        type: Array,
        default: () => ['uiProperty']
      },
      route: {
        type: [String, Object],
        default: null
      }
    },

    data: () => ({
      dirty: false,
      loadingState: 'default',
      loadingError: null,
      state: 'default',
      errors: [],
      canEdit: true,
      slotProps: {
        state: null
      },
      submitBlocked: false
    }),

    watch: {
      '$route': {
        deep: true,
        handler: function (val)
        {
          this.$nextTick(() =>
          {
            this.$emit('load', this);
          });
        }
      },
      state(val)
      {
        this.slotProps.state = val;
      },
      canEdit(val)
      {
        this.$nextTick(() =>
        {
          this.setCanEdit(val);
        });
      }
    },


    created()
    {
      this.slotProps.state = this.state;
      this.$emit('load', this);
    },


    methods: {

      // shows a confirmation dialog for dirty forms when the route tries to change
      // it only works when this method is attached to the route component
      beforeRouteLeave(to, from, next) 
      {
        if (this.dirty)
        {
          Overlay.confirm({
            title: '@unsavedchanges.title',
            text: '@unsavedchanges.text',
            confirmLabel: '@unsavedchanges.confirm',
            closeLabel: '@unsavedchanges.close'
          }).then(
            () => next(false),
            () =>
            {
              this.dirty = false;
              next();
            }
          );
        }
        else
        {
          next()
        }
      },

      // loads data on creation of the form
      async load(promise)
      {
        this.setState('loading');
        const response = await promise();

        if (!response.success)
        {
          this.setState('error');
          if (response.errors)
          {
            this.loadingError = response.errors[0].message;
          }
          return null;
        }

        this.canEdit = true;
        this.setState('default');
        this.$nextTick(() =>
        {
          this.$emit('loaded', this);
        });
        return response.data;
      },

      // handles a promise as result of the form submission
      async handle(response, isCreate)
      {
        this.setState('loading');
        this.clearErrors();

        console.info(response);

        if (!response.success)
        {
          this.setState('error');
          this.setErrors(response.errors);
          return null;
        }

        this.setState('success');
        this.setDirty(false);

        if (response.data && this.route && response.status === 201)
        {
          let routeObj = typeof this.route === 'object' ? this.route : { name: this.route };
          routeObj.params = routeObj.params || {};
          routeObj.query = this.$route.query || {};
          routeObj.params.id = response.data.id;

          this.$router.replace(routeObj);
        }
      },


      // submits the form
      onSubmit(e)
      {
        if (!this.submitBlocked)
        {
          this.$emit('submit', this, e);
        }
      },


      // set the form to dirty when one of the fields changes
      onChange(e)
      {
        this.dirty = true;
      },


      // handle delete event
      async onDelete(promise)
      {
        const overlay = await overlays.confirmDelete();

        if (overlay.eventType == 'close')
        {
          return;
        }

        const opts = overlay.value;

        opts.state('loading');
        const response = await promise();

        if (response.success)
        {
          opts.state('success');
          opts.close();
          this.$router.go(-1);
          notifications.success('@deleteoverlay.success', '@deleteoverlay.success_text');
        }
        else
        {
          opts.state('error');
          opts.errors(response.errors);
        }
      },


      // prevent submission of form on enter key press
      onKeydown(e)
      {
        if (e.keyCode === 13)
        {
          this.submitBlocked = true;
          clearTimeout(this.submitBlockedTimeout);
          this.submitBlockedTimeout = setTimeout(() => this.submitBlocked = false, 300);
        }
      },


      // set dirty status
      setDirty(dirty)
      {
        this.dirty = dirty;
      },


      // set state for this component and all child components which attached it's state from the slot props
      setState(state)
      {
        this.state = state;
      },


      // clears all errors from the form
      clearErrors()
      {
        this.getErrorComponents().forEach(component =>
        {
          component.clearErrors();

          if (component.tab)
          {
            component.tab.clearErrors();
          }
        });
      },


      // tries to find matching fields for the given errors and displays them
      setErrors(errors)
      {
        if (typeof errors === 'undefined' || !errors)
        {
          this.errors = [];
        }
        else
        {
          this.errors = !Array.isArray(errors) ? [errors] : errors;
        }

        // get all components + grouped errors
        let errorComponents = this.getErrorComponents();
        let errorGroups = arrayGroupBy(this.errors, 'property');
        let handledGroups = [];

        // set errors
        errorComponents.forEach(component =>
        {
          let field = component.field;

          if (field && errorGroups[field])
          {
            handledGroups.push(field);
            component.setErrors(errorGroups[field]);

            if (component.tab)
            {
              component.tab.setErrors(true);
            }
          }
        });

        for (var field in errorGroups)
        {
          let errorGroup = errorGroups[field];
          if (handledGroups.indexOf(field) < 0)
          {
            errorComponents.forEach(component =>
            {
              if (component.catchRemaining || component.catchAll)
              {
                component.setErrors(errorGroup, true);

                if (component.tab)
                {
                  component.tab.setErrors(true);
                }
              }
            });
          }
        }
      },


      // find all error components in form
      getErrorComponents()
      {
        let errorComponents = [];

        // find components which can output errors
        let traverseChildren = (parent, tab) =>
        {
          parent.$children.forEach(component =>
          {
            if (this.errorComponents.indexOf(component.$options.name) > -1)
            {
              errorComponents.push({
                name: component.$options.name,
                field: component.field,
                catchAll: component.catchAll,
                catchRemaining: component.catchRemaining,
                component: component,
                setErrors: component.setErrors,
                clearErrors: component.clearErrors,
                tab: tab
              });
            }
            else
            {
              traverseChildren(component, tab || (component.$options.name === 'uiTab' ? component : null));
            }
          });
        };

        traverseChildren(this);

        return errorComponents;
      },


      // sets the form editing ability
      setCanEdit(canEdit)
      {
        // find components which can output errors
        //let traverseChildren = parent =>
        //{
        //  parent.$children.forEach(component =>
        //  {
        //    const isValidComponent = this.inputComponents.indexOf(component.$options.name) > -1;
        //    //const field = component.field;

        //    if (typeof component._props.disabled === 'boolean')
        //    {
        //      component.$emit('update:disabled', canEdit);
        //    }
        //    else
        //    {
        //      traverseChildren(component);
        //    }
        //    //if (isErrorComponent)
        //    //{
        //    //  errorComponents.push(component);
        //    //}

        //    //if (isErrorComponent && field)
        //    //{
        //    //  let errorGroup = errorGroups[field];

        //    //  if (errorGroup)
        //    //  {
        //    //    handledGroups.push(field);
        //    //    component.set(errorGroup);
        //    //  }
        //    //}
        //    //else
        //    //{
        //    //  traverseChildren(component);
        //    //}
        //  });
        //};

        //traverseChildren(this);

        //console.info('done');
      }
    }
  });
</script>

<style lang="scss">
  .ui-form
  {
    min-height: 100%;
    font-size: var(--font-size);
  }

  .ui-form-loading
  {
    display: flex;
    width: 100%;
    height: 100vh;
    align-items: center;
    justify-content: center;
  }

  .ui-form-loading-progress
  {   
    width: 32px;
    height: 32px;
    z-index: 2;
    border-radius: 40px;
    border: 2px solid var(--color-bg-shade-3);
    border-left-color: var(--color-text);
    opacity: 1;
    will-change: transform;
    animation: rotating .5s linear infinite;
    transition: opacity .25s ease;
  }

  @keyframes rotating
  {
    from
    {
      -webkit-transform: rotate(0);
      transform: rotate(0)
    }
    to
    {
      -webkit-transform: rotate(1turn);
      transform: rotate(1turn)
    }
  }
</style>