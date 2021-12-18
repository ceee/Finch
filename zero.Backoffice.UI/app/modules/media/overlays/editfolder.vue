<template>
  <ui-form ref="form" class="mediafolder" v-slot="form" @submit="onSubmit" @load="onLoad">
    <div v-if="!loading">
      <h2 class="ui-headline" v-localize="model.id ? '@media.editfolder' : '@media.addfolder'"></h2>
      <div class="mediafolder-items">
        <ui-property :required="true">
          <input v-model="item.name" type="text" class="ui-input" maxlength="80" v-localize:placeholder="'@media.fields.foldername_placeholder'" :disabled="disabled" />
          <ui-error :catch-all="true" />
        </ui-property>
      </div>
      <div class="app-confirm-buttons">
        <ui-button type="accent" v-if="!disabled" :submit="true" :state="form.state" :label="model.id ? '@ui.update' : '@ui.create'"></ui-button>
        <ui-button type="light" label="@ui.close" :disabled="loading" @click="config.close"></ui-button>
      </div>
    </div>
  </ui-form>
</template>


<script>
  import api from '../api';

  export default {

    props: {
      model: Object,
      config: Object
    },

    data: () => ({
      loading: true,
      item: { name: null },
      disabled: false
    }),

    methods: {


      async onLoad(form)
      {
        const id = this.model.id;
        const response = await form.load(() => id ? api.folders.getById(id) : api.folders.getEmpty(this.$route.query['zero.flavor']));
        this.item = response;
        this.item.parentId = this.model.parentId || null;
        this.loading = false;
      },


      async onSubmit(form)
      {
        form.setState('loading');
        const response = this.model.id ? await api.folders.update(this.item) : await api.folders.create(this.item);
        await form.handle(response);

        if (response.success)
        {
          this.config.confirm(response.data);
        }
      }
    }
  }
</script>

<style lang="scss">
  .mediafolder
  {
    text-align: left;
  }

  .mediafolder-items
  {
    margin-top: var(--padding);
  }
</style>