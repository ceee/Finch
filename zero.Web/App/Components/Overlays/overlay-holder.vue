<template>
  <div class="app-overlays" :class="{ 'has-multiple': multiple }">
    <div v-for="instance in instances">
      <template>{{instance.key}}</template>
    </div>
    <!--<ui-content-dialog v-for="instance in instances" :key="instance.id" :content="instance.data" :component="instance.component" />-->
    <!--<ui-overlay v-for="instance in instances" :key="instance.id" :content="instance.data" :component="instance.component" />-->
  </div>
</template>


<script>
  import Overlay from 'zeroservices/overlay.js'
  import Strings from 'zeroservices/strings.js'

  export default {
    data: () => ({
      instances: []
    }),

    computed: {
      multiple()
      {
        return this.instances.length > 1;
      }
    },

    created ()
    {
      Overlay.$on('open', opts => this.open(opts));

      //Overlay.$on('close', )

      //EventHub.$on('overlay', (component, data) =>
      //{
      //  data.onClose = this.close;

      //  this.instances.push({
      //    id: Strings.guid(),
      //    component: component,
      //    data: data
      //  });
      //});

      //EventHub.$on('overlay-close', () =>
      //{
      //  if (this.instances.length > 0)
      //  {
      //    this.close();
      //  }    
      //});
    },

    methods: {

      open(options)
      {
        const key = Strings.guid();

        let instance = {
          key: key,
          component: options.component,
          model: options.model,
          options: options
        };

        this.instances.push(instance);
      },

      close()
      {
        //var instance = this.instances[this.instances.length - 1];
        this.instances.pop();
      }
    }
  }
</script>


<style lang="scss">
  .app-overlays
  {
    grid-column: span 2 / auto;
  }
</style>