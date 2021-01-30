
export default {
  version: '0.1.0',

  apiPath: '/zero/api/',

  path: '/zero/',

  media: {
    defaults: {
      images: ['.jpg', '.jpeg', '.png', '.webp', '.svg'],
      images_natural: ['.jpg', '.jpeg', '.webp'],
      images_artificial: ['.png', '.webp', '.svg']
    }
  },

  linkPicker: {
    areas: [
      {
        alias: 'zero.pages',
        name: '@zero.config.linkareas.pages',
        display: 'tree'
      },
      {
        alias: 'zero.media',
        name: '@zero.config.linkareas.media',
        display: 'media'
      }
    ]
  }
}