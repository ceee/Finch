import Vue from 'vue';
import Strings from 'zero/helpers/strings.js';
import { find as _find, extend as _extend } from 'underscore';

export default new Vue({

  data: () => ({
    instances: []
  }),

  methods: {

    success(label, text, options)
    {
      this.show(_extend({
        type: 'success',
        label: label,
        text: text
      }, options || {}));
    },


    error(label, text, options)
    {
      this.show(_extend({
        type: 'error',
        label: label,
        text: text
      }, options || {}));
    },


    show(options, text)
    {
      let me = this;

      if (typeof options === 'string')
      {
        options = {
          label: options,
          text: text
        };
      }

      options = _extend({
        id: Strings.guid(),
        type: 'default',
        label: null,
        text: null,
        persistent: false,
        duration: 3000,
        timeout: null,
        close: () =>
        {
          clearTimeout(options.timeout);
          me.close(this.instances.indexOf(options));
        }
      }, options);

      if (!options.label && !options.text)
      {
        return;
      }

      options.timeout = setTimeout(() =>
      {
        options.close();
      }, options.duration);

      this.instances.push(options);
    },


    close(index)
    {
      if (index > -1)
      {
        this.instances.splice(index, 1);
      }
      else
      {
        this.instances = [];
      }
    }
  }
});