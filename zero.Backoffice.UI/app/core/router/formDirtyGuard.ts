import { RouteLocationNormalized, NavigationGuardNext, nav } from 'vue-router';

export default function (to: RouteLocationNormalized, from: RouteLocationNormalized, next: NavigationGuardNext)
{
  if (from.matched.length && from.matched[0].instances)
  {
    let instance = from.matched[0].instances.default;

    if (instance.$refs['form'] && typeof instance.$refs.form.beforeRouteLeave === 'function')
    {
      isGuarded = true;
      instance.$refs.form.beforeRouteLeave(to, from, res =>
      {
        next(res !== false);
      });
    }
  }
}