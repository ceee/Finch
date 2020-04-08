import Vue from 'vue';
import AppConfirm from 'zerocomponents/Overlays/confirm';

export default new Vue({
  methods: {

    // open a confirm dialog with the given options
    confirm(title, text)
    {
      let options = typeof title === 'object' ? title : {
        title: title,
        text: text,
        component: AppConfirm
      };

      this.open(options);
    },

    // opens a dialog
    open(options)
    {
      this.$emit('open', options);

      return new Promise((resolve, reject) =>
      {

      });
    }

  }
});