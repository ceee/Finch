

import { Zero } from '../types/zero';
import { createWebHistory, Router, RouterOptions } from 'vue-router';
import titleGuard from './titleGuard';
import formDirtyGuard from './formDirtyGuard';

export function getRouterConfig(basePath: string, zero: Zero): RouterOptions
{
  let options = {
    history: createWebHistory(basePath),
    linkActiveClass: 'is-active',
    linkExactActiveClass: 'is-active-exact',
    scrollBehavior(to, from, savedPosition)
    {
      return savedPosition ? savedPosition : { x: 0, y: 0 };
    },
    routes: []
  } as RouterOptions;

  options.routes.push({ name: 'dashboard', path: '/', component: () => import('../../dashboard.vue') });
  options.routes.push({ name: '404', path: '/:pathMatch(.*)', component: () => import('../../notfound.vue') });

  return options;
}


export function appendRouterGuards(router: Router): Router
{
  //router.beforeEach(formDirtyGuard);
  //router.beforeEach(titleGuard);

  return router;
}