<template>
  <!-- // TODO notfound view works and can be used here -->
  <div class="page page-error" :class="{'theme-dark': dark }">
    <i class="page-error-icon fth-cloud-snow"></i>
    <p class="page-error-text">
      <strong class="page-error-headline" v-localize="{ key: errorDetails.headline, tokens: tokens }"></strong><br>
      <span v-localize:html="{ key: errorDetails.text, tokens: tokens }"></span>
    </p>
    <ui-button v-if="errorDetails.code === 404" class="page-error-button" type="light onbg" :label="detailsText" @click="details = !details" />
    <ui-button v-if="errorDetails.code !== 404 && errorDetails.category === 4" class="page-error-button" type="light onbg" label="@ui.back" @click="$router.go(-1)" />
    <!--<ui-button v-if="errorDetails.code !== 404 && errorDetails.category === 5" class="page-error-button" type="light" label="@ui.back" @click="$router.go(-1)" />-->
    <template v-if="errorDetails.code === 404 && details">
      <br><br>
      <div class="page-error-routes">
        <span>#</span>
        <span>Name</span>
        <span>Path</span>
        <template v-for="(route, index) in routes">
          <span>{{index + 1}}.</span>
          <span>{{route.name}}</span>
          <span>{{route.path}}</span>
        </template>
      </div>
    </template>
  </div>
</template>


<script>
  const KNOWN_ERRORS = [403, 404, 409, 504];

  export default {

    props: {
      dark: {
        type: Boolean,
        default: false
      },
      error: {
        type: Error,
        default: null
      }
    },

    data: () => ({
      path: null,
      routes: [],
      details: false,
      errorDetails: {
        category: 0,
        code: null,
        headline: null,
        text: null
      }
    }),

    watch: {
      error: function (val)
      {
        this.rebuild();
      },
      '$route': function (val)
      {
        this.rebuild();
      }
    },

    computed: {
      detailsText()
      {
        return this.details ? 'Hide' : 'Defined routes';
      },
      tokens()
      {
        return {
          code: this.errorDetails.code,
          path: this.path
        };
      }
    },

    mounted()
    {
      console.info(this.error.response);
      this.rebuild();
    },

    methods: {

      rebuild()
      {
        if (this.error && this.error.response)
        {
          let errorKey = null;

          const errorCode = this.error.response.status;
          const errorCategory = Math.round(errorCode / 100);

          if (KNOWN_ERRORS.indexOf(errorCode) > -1)
          {
            errorKey = '@errors.http.' + errorCode;
          }
          else if (errorCategory === 4)
          {
            errorKey = '@errors.http.4xx';
          }
          else if (errorCategory === 5)
          {
            errorKey = '@errors.http.5xx';
          }

          this.errorDetails.category = errorCategory;
          this.errorDetails.code = errorCode;
          this.errorDetails.headline = errorKey;
          this.errorDetails.text = errorKey + '_text';
        }

        this.path = this.$router.options.base + this.$route.path.substring(1);
        this.routes = [];

        this.$router.options.routes.forEach(route =>
        {
          this.routes.push({
            path: route.path,
            name: route.name
          });

          if (route.children)
          {
            route.children.forEach(child =>
            {
              this.routes.push({
                path: route.path + '/' + child.path,
                name: child.name
              });
            });
          }
        });
      }

    }

  }
</script>

<style lang="scss">
  .page-error
  {
    width: 100%;
    min-height: 100vh;
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    color: var(--color-text);
    text-align: center;
    padding: var(--padding);
    overflow-y: auto;
  }

  .page-error-icon
  {
    font-size: 82px;
    color: var(--color-text);
    margin-bottom: 20px;
  }

  .page-error-text
  {
    font-size: var(--font-size);
    color: var(--color-text-dim);
    line-height: 1.4em;
  }

  .page-error-headline
  {
    display: inline-block;
    margin-bottom: 10px;
    font-size: var(--font-size-l);
    color: var(--color-text);
  }

  .page-error-button
  {
    margin-top: 10px;
  }

  .page-error-routes
  {
    display: grid;
    grid-template-columns: auto auto 1fr;
    width: 100%;
    max-width: 100%;
    text-align: left;
    border-top: 1px solid var(--color-line);
    border-left: 1px solid var(--color-line);
    margin-top: 30px;

    span
    {
      border: 1px solid var(--color-line);
      border-left: none;
      border-top: none;
      padding: 8px 10px 6px;
      font-family: Consolas;
      font-size: 12px;

      &:nth-child(3n+1)
      {
        color: var(--color-text-dim);
      }
    }
  }
</style>