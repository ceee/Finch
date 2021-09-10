import Vue from 'vue';
import AppConfirm from 'zero/components/overlays/confirm.vue'; // TODO importing vue files in js/ts files causes a Rollup production build error
import Strings from 'zero/helpers/strings.js';
import { find as _find, extend as _extend } from 'underscore';

export default new Vue({

  data: () => ({
    dropdownInstance: null,
    instances: []
  }),

  methods: {

    // sets a new active dropdown so the old one gets auto-closed
    setDropdown(instance)
    {
      if (this.dropdownInstance != null)
      {
        this.dropdownInstance.hide();
      }

      this.dropdownInstance = instance;
    },

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
        component: AppConfirm,
        autoclose: true,
        softdismiss: false
      }, typeof title === 'object' ? title : {});

      return this.open(options);
    },


    // opens an overlay
    open(options)
    {
      const defaultWidth = options.display === 'editor' ? 560 : 460;

      options = _extend({
        id: Strings.guid(),
        display: 'dialog',
        width: defaultWidth,
        hide: this.close,
        autoclose: true,
        softdismiss: options.display !== 'editor',
        closeLabel: '@ui.close',
        confirmLabel: '@ui.confirm',
        confirmType: 'default',
        alias: options.alias
      }, options);

      if (typeof options.theme === 'undefined')
      {
        options.theme = 'default';
      }

      this.instances.push(options);

      return new Promise((resolve, reject) =>
      {
        options.close = () =>
        {
          this.close(options);
          //reject(options);
          // TODO should we move to resolve here, so we don't trigger errors in case the implementation does not catch them?
          // this will at least need some tests if the .then callback does not catch null values
        };
        options.hide = options.close;

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