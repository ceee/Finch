<template>
  <div v-if="value" class="ui-linkpicker-area-url">
    <ui-property label="@links.fields.url.url" :required="true">
      <input :value="url" @input="onUpdateUrl($event.target.value)" type="url" class="ui-input" :maxlength="240" />
    </ui-property>
    <ui-property label="@links.fields.target">
      <ui-linktarget v-model:value="value.target" @input="onUpdate" :disabled="disabled" />
    </ui-property>
    <ui-property label="@links.fields.title" description="@links.fields.title_text">
      <input v-model="value.title" type="text" class="ui-input" @input="onUpdate" :maxlength="120" />
    </ui-property>
  </div>
</template>


<script lang="ts">
  export default {
    name: 'uiLinkpickerAreaUrl',

    props: {
      value: {
        type: Object,
        required: true
      },
      area: {
        type: Object,
        required: true
      }
    },

    data: () => ({
      url: null
    }),

    watch: {
      'value.values.url'(val)
      {
        this.url = val;
      }
    },

    mounted()
    {
      this.url = this.value.values['url'];
    },

    methods: {

      onUpdateUrl(url)
      {
        this.value.values = { url };
        this.onUpdate();
      },


      onUpdate()
      {
        this.$emit('change', this.value);
        this.$emit('input', this.value);
        this.$emit('update:value', this.value);
      }
    }
  }
</script>

<style lang="scss">
  
</style>