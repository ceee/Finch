
import { App } from 'vue';
import * as components from './index';

export default function (app: App)
{
  for (var key in components)
  {
    let component = components[key];
    app.component(key, component.default || component);
  }
};