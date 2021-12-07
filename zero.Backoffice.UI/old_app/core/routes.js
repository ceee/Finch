
import dashboardRoutes from '../pages/dashboard/routes.js';
import pageRoutes from '../pages/pages/routes.js';
import mediaRoutes from '../pages/media/routes.js';
import settingsRoutes from '../pages/settings/routes.js';
import spaceRoutes from '../pages/spaces/routes.js';

export default [
  {
    name: 'preview',
    path: '/preview',
    component: () => import('../pages/preview.vue'),
    meta: {
      name: '@preview.name'
    }
  },
  ...dashboardRoutes,
  ...pageRoutes,
  ...mediaRoutes,
  ...settingsRoutes,
  ...spaceRoutes,
  {
    name: '404',
    path: '*',
    component: () => import('../pages/notfound.vue')
  }
];