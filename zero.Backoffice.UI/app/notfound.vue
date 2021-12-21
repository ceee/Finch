<template>
  <div class="page page-error">
    <ui-icon class="page-error-icon" symbol="fth-cloud-snow" :size="82" />
    <p class="page-error-text">
      <strong class="page-error-headline">Not found</strong><br>
      The requested resource could not be found
      <br>
      <code>{{path}}</code>
    </p>
    <ui-button class="page-error-button" type="light onbg" :label="detailsText" @click="details = !details" />
    <template v-if="details">
      <br><br>
      <div class="page-error-routes">
        <span>#</span>
        <span>Name</span>
        <span>Path</span>
        <template v-for="(route, index) in routes" :key="index">
          <span>{{index + 1}}.</span>
          <b>{{route.name}} <em v-if="route.parent">[parent:{{route.parent}}]</em></b>
          <span>{{route.path}}</span>
        </template>
      </div>
    </template>
  </div>
</template>


<script>
  import { defineComponent } from "vue";

  export default defineComponent({

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
        this.path = this.$route.path;
        this.routes = [];

        let routes = [];

        console.info(this.$router.getRoutes().reverse());

        this.$router.getRoutes().reverse().forEach(route =>
        {
          if (!routes.find(x => x.path == route.path && x.name == route.name && x.parent))
          {
            routes.push({
              path: route.path,
              name: route.name,
              parent: null
            });
          }

          if (route.children)
          {
            route.children.forEach(child =>
            {
              routes.push({
                path: route.path + '/' + child.path,
                name: child.name,
                parent: route.name
              });
            });
          }
        });

        this.routes = routes.sort((a, b) => a.path > b.path ? 1 : -1);
      }
    }
  })
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
    color: var(--color-text);
    text-align: center;
    padding: var(--padding);
    overflow-y: auto;
  }

  .page-error-icon
  {
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
    margin-top: var(--padding);
  }

  .page-error-routes
  {
    display: grid;
    grid-template-columns: auto auto 1fr;
    width: 100%;
    max-width: 100%;
    text-align: left;
    border-top: 1px solid var(--color-line-onbg);
    border-left: 1px solid var(--color-line-onbg);
    margin-top: 30px;

    span, b
    {
      border: 1px solid var(--color-line-onbg);
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

    em
    {
      font-style: normal;
      font-size: 10px;
      color: var(--color-text-dim);
    }
  }
</style>