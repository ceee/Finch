import Vue from 'vue';
import AppConfirm from 'zero/components/overlays/confirm';
import Strings from 'zero/services/strings';
import { find as _find, extend as _extend } from 'underscore';

export default new Vue({

  data: () => ({
    instances: []
  }),

  methods: {

    // open a deletion confirm dialog with the given options
    confirmDelete(title, text)
    {
      let options = _extend({
        title: typeof title === 'string' ? title : '@deleteoverlay.title',
        text: text || '@deleteoverlay.text',
        confirmLabel: '@deleteoverlay.confirm',
        confirmType: 'danger',
        closeLabel: '@deleteoverlay.close',
        component: AppConfirm,
        autoclose: false,
        softdismiss: false
      }, typeof title === 'object' ? title : {});

      return this.open(options);
    },

    // open a confirm dialog with the given options
    confirm(title, text)
    {
      let options = _extend({
        title: title,
        text: text,
        confirmLabel: '@ui.confirm',
        confirmType: 'default',
        closeLabel: '@ui.close',
        component: AppConfirm,
        autoclose: true,
        softdismiss: false
      }, typeof title === 'object' ? title : {});

      return this.open(options);
    },


    // opens an overlay
    open(options)
    {
      options.id = Strings.guid();

      options.hide = this.close;

      this.instances.push(options);

      return new Promise((resolve, reject) =>
      {
        options.close = () =>
        {
          this.close(options);
          reject(options);
        };
        options.confirm = data =>
        {
          if (options.autoclose)
          {
            this.close(options);
          }
          resolve(data, options);
        };
      });
    },


    // closes an overlay
    close(instance)
    {
      if (this.instances.length < 1)
      {
        return;
      }

      if (!instance)
      {
        this.instances.pop();
        return;
      }

      if (typeof instance === 'string')
      {
        instance = _find(this.instances, item => item.id === instance);
      }

      if (instance)
      {
        const index = this.instances.indexOf(instance);
        this.instances.splice(index, 1);
      }
    },

    // closes all overlays
    closeAll()
    {
      this.instances.forEach(instance =>
      {

      });
    }

  }
});