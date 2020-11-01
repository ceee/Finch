import Localization from 'zero/services/localization.js';
import { isArray as _isArray } from 'underscore';


// set meta title before routing
const beforeEach = (to, from, next) =>
{
  let isGuarded = false;

  // set document title + call next
  let callback = () =>
  {
    let title = Localization.localize('@zero.name');
    let name = to.meta.name;

    if (!name && to.matched.length > 1)
    {
      to.matched.forEach(route =>
      {
        if (!name && route.meta.name)
        {
          name = route.meta.name;
        }
      });
    }

    if (!name || to.meta.alias === __zero.alias.sections.dashboard)
    {
      document.title = title;
      next();
      return;
    }

    let nameParts = _isArray(name) ? name : [name];
    let translations = [];

    nameParts.forEach(part =>
    {
      if (part)
      {
        translations.push(Localization.localize(part));
      }
    });

    title += ': ' + translations.join(' - ');

    document.title = title;

    next();
  };

  // dirty form guard
  if (from.matched.length && from.matched[0].instances)
  {
    let instance = from.matched[0].instances.default;

    if (instance.$refs['form'] && typeof instance.$refs.form.beforeRouteLeave === 'function')
    {
      isGuarded = true;
      instance.$refs.form.beforeRouteLeave(to, from, res =>
      {
        if (res === false)
        {
          next(false);
        }
        else
        {
          callback();
        }
      });
    }
  }

  if (!isGuarded)
  {
    callback();
  }
};


export default {
  mode: 'history',
  base: __zero.path,
  linkActiveClass: 'is-active',
  linkExactActiveClass: 'is-active-exact',
  beforeEach: beforeEach,
  scrollBehavior(to, from, savedPosition)
  {
    return savedPosition ? savedPosition : { x: 0, y: 0 };
  }
};