
const alias = __zero.alias.sections.settings;
const section = __zero.sections.find(x => x.alias === alias);
let settings = [];
let routes = [];

__zero.settingsAreas.forEach(group => group.items.forEach(area =>
{
  if (!area.isPlugin)
  {
    settings.push(area);
  }
}));

const addArea = (areaAlias, component, detailComponent, hasCreate, postCreate) =>
{
  let area = typeof areaAlias === 'object' ? areaAlias : settings.find(x => x.alias === areaAlias);

  if (!area)
  {
    return;
  }

  routes.push({
    name: area.alias,
    path: area.url,
    component: component,
    meta: {
      name: area.name
    }
  });

  if (detailComponent && hasCreate)
  {
    routes.push({
      name: area.alias + '-create',
      path: area.url + '/create/:scope?',
      props: true,
      component: detailComponent,
      meta: {
        create: true,
        name: area.name
      }
    });
  }

  if (detailComponent)
  {
    routes.push({
      name: area.alias + '-edit',
      path: area.url + '/edit/:id',
      props: true,
      component: detailComponent,
      meta: {
        name: area.name
      }
    });
  }

  if (typeof postCreate === 'function')
  {
    postCreate(area);
  }
};


if (section)
{
  // add overview page
  routes.push({
    name: section.alias,
    path: section.url,
    component: () => import('./settings.vue'),
    meta: {
      name: section.name,
      alias: section.alias,
      section: section
    }
  });


  // add details

  addArea(__zero.alias.settings.applications, () => import('./applications.vue'), () => import('./application.vue'), true);

  addArea(__zero.alias.settings.countries, () => import('./countries.vue'), () => import('./country.vue'), true);

  addArea(__zero.alias.settings.languages, () => import('./languages.vue'), () => import('./language.vue'), true);

  addArea(__zero.alias.settings.translations, () => import('./translations.vue'), () => import('./translations.vue'), true);

  addArea(__zero.alias.settings.mails, () => import('./mails.vue'), () => import('./mail.vue'), true);

  addArea(__zero.alias.settings.integrations, () => import('./integrations.vue'));

  addArea(__zero.alias.settings.users, () => import('./users.vue'), () => import('./user.vue'), true, area =>
  {
    routes.push({
      name: 'roles-create',
      path: '/' + section.alias + '/roles/create/:scope?',
      props: true,
      component: () => import('./role.vue'),
      meta: {
        create: true,
        name: area.name
      }
    });

    routes.push({
      name: 'roles-edit',
      path: '/' + section.alias + '/roles/edit/:id',
      props: true,
      component: () => import('./role.vue'),
      meta: {
        name: area.name
      }
    });
  });
}


export default routes;