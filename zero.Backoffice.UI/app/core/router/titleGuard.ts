import { localize } from '../../services/localization';
import { RouteLocationNormalized, NavigationGuardNext } from 'vue-router';

export default function (to: RouteLocationNormalized, from: RouteLocationNormalized, next: NavigationGuardNext)
{
  let title = localize('@zero.name');
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

  if (!name || to.meta.alias === 'dashboard') //__zero.alias.sections.dashboard)
  {
    document.title = title;
    next();
    return;
  }

  let nameParts = Array.isArray(name) ? name : [name];
  let translations = [];

  nameParts.forEach(part =>
  {
    if (part)
    {
      translations.push(localize(part));
    }
  });

  title += ': ' + translations.join(' - ');

  document.title = title;

  next();
}