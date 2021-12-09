
import { App } from 'vue';
import * as partials from './components/index';
import * as components from './index';

export default function (app: App)
{
  for (var key in partials)
  {
    let component = partials[key];
    app.component(key, component.default || component);
  }

  for (var key in components)
  {
    let component = components[key];
    app.component(key, component.default || component);
  }
};