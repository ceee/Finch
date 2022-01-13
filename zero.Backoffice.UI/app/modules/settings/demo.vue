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
        icon: 'fth-zap',
        number: 17,
        output: 'This is an output',
        rte: '<p><b>Mudita Center is privacy-focused</b> - it will not store nor share any contact details or any other data with Mudita nor any third parties.</p><p>It is also <u>open-sourced</u> for transparency and available for all three major operating systems - Windows, macOS, and Linux.</p>',
        textarea: 'A textarea\nfor you',
        state: 'red',
        images: ['media.zpkxfjkgciom', 'media.w2cntwlrcueb', 'media.1waxp46ho8hu'],
        docs: 'media.xoooo',
        productId1: null,
        productId2: [],
        productId3: null,
        link: null
      },
      route: 'demo',
      disabled: false
    })
  }
</script>