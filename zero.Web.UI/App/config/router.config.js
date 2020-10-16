import Localization from 'zero/services/localization';


// set meta title before routing
const beforeEach = () => (to, from, next) =>
{
  let isGuarded = false;

  // set document title + call next
  let callback = () =>
  {
    let title = Localization.localize('@zero.name');

    if (to.meta.name && to.meta.alias !== zero.alias.sections.dashboard)
    {
      let name = to.meta.name;
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
    }

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
  base: zero.path,
  linkActiveClass: 'is-active',
  linkExactActiveClass: 'is-active-exact',
  beforeEach: beforeEach,
  scrollBehavior(to, from, savedPosition)
  {
    return savedPosition ? savedPosition : { x: 0, y: 0 };
  }
};