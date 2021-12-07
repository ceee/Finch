

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

  //options.routes.push({
  //  name: 'xroot',
  //  path: '/',
  //  component: () => import('../../_root.vue'),
  //  meta: {
  //    name: 'Root name',
  //    alias: 'rootalias',
  //    section: 'Root section'
  //  }
  //} as RouteRecordRaw);

  options.routes.push({ path: '/', component: () => import('../../_root.vue') });
  options.routes.push({ path: '/about', component: () => import('../../_about.vue') });

  // TODO add zero routes

  return options;
}


export function appendRouterGuards(router: Router): Router
{
  //router.beforeEach(formDirtyGuard);
  //router.beforeEach(titleGuard);

  return router;
}