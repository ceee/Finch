<template>
  <div>
    <ui-header-bar title="Demo" />
    <ui-editor config="demo" v-model="model" :meta="meta" :disabled="disabled" />
  </div>
</template>


<script>
  //import api from './api';

  export default {
    data: () => ({
      meta: {},
      model: {
        name: 'Tobi',
        isActive: true,
        number: 17,
        output: 'This is an output',
        rte: '',
        textarea: 'A textarea\nfor you',
        state: 'red',
        images: ['media.zpkxfjkgciom', 'media.w2cntwlrcueb', 'media.1waxp46ho8hu'],
        docs: null
      },
      route: 'demo',
      disabled: false
    })
  }
</script>