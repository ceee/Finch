import Vue from 'vue';
import Strings from 'zero/services/strings';
import { find as _find, extend as _extend } from 'underscore';

export default new Vue({

  data: () => ({
    instances: []
  }),

  methods: {

    success(label, text)
    {
      this.show({
        type: 'success',
        label: label,
        text: text
      });
    },


    error(label, text)
    {
      this.show({
        type: 'error',
        label: label,
        text: text
      });
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