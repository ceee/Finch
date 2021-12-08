import { ZeroPlugin, ZeroPluginOptions } from '../../core';
import { defineAsyncComponent } from 'vue';
import { test } from '../../editor/tests/types';
import { proxy, fieldTypes } from '../../editor/editorFieldProxy';
import { TextFieldOptions } from 'zero/editor';

const Picker = () => import('./ui-countrypicker.vue');
const Page = () => import('./_page.vue');

export default {
  name: "zero.countries",

  install(app: ZeroPluginOptions)
  {
    app.vue.component('ui-countrypicker', defineAsyncComponent(Picker));
    app.route({ path: '/countries', component: Page });

    //app.editor('country', null);
    //app.editorField('')
    test();

    fieldTypes.number = (maxLength?: number, placeholder?: string | Function) => console.log(`number() called with maxLength: ${maxLength}, placeholder: ${placeholder}`);

    proxy.number({ maxLength: 17, placeholder: 'Enter your number...' });
  }
} as ZeroPlugin;