<template>
  <div class="page page-error">
    <i class="page-error-icon fth-cloud-snow"></i>
    <p class="page-error-text">
      <strong class="page-error-headline">Not found</strong><br>
      The requested resource could not be found
      <br>
      ({{path}})
    </p>
    <ui-button class="page-error-button" type="white" :label="detailsText" @click="details = !details" />
    <template v-if="details">
      <br><br>
      <div class="page-error-routes">
        <span>#</span>
        <span>Name</span>
        <span>Path</span>
        <template v-for="(route, index) in routes">
          <span>{{index + 1}}.</span>
          <b>{{route.name}}</b>
          <span>{{route.path}}</span>
        </template>
      </div>
    </template>
  </div>
</template>


<script>
  export default {

    data: () => ({
      path: null,
      routes: [],
      details: false
    }),

    watch: {
      '$route': function (val)
      {
        this.rebuild();
      }
    },

    computed: {
      detailsText()
      {
        return this.details ? 'Hide' : 'Defined routes';
      }
    },

    mounted()
    {
      this.rebuild();
    },

    methods: {

      rebuild()
      {
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
    min-height: 100%;
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    background: var(--color-bg);
    color: var(--color-fg);
    text-align: center;
    padding: var(--padding);
    overflow-y: auto;
  }

  .page-error-icon
  {
    font-size: 82px;
    color: var(--color-fg);
    margin-bottom: 20px;
  }

  .page-error-text
  {
    font-size: var(--font-size);
    color: var(--color-fg-dim);
    line-height: 1.4em;
  }

  .page-error-headline
  {
    display: inline-block;
    margin-bottom: 10px;
    font-size: var(--font-size-l);
    color: var(--color-fg);
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

    span, b
    {
      border: 1px solid var(--color-line);
      border-left: none;
      border-top: none;
      padding: 8px 10px 6px;
      font-family: Consolas;
      font-size: 12px;

      &:nth-child(3n+1)
      {
        color: var(--color-fg-dim);
      }
    }
  }
</style>