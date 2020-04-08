import Vue from 'vue';
import AppConfirm from 'zerocomponents/Overlays/confirm';
import Strings from 'zeroservices/strings';
import { find as _find } from 'underscore';

export default new Vue({

  data: () => ({
    instances: []
  }),

  methods: {

    // open a confirm dialog with the given options
    confirm(title, text)
    {
      let options = typeof title === 'object' ? title : {
        model: {
          title: title,
          text: text
        },
        component: AppConfirm
      };

      return this.open(options);
    },


    // opens an overlay
    open(options)
    {
      options.id = Strings.guid();

      this.instances.push(options);

      return new Promise((resolve, reject) =>
      {
        options.close = () =>
        {
          this.close(options);
          reject();
        };
        options.confirm = data =>
        {
          this.close(options);
          resolve(data);
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