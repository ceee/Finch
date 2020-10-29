
import Settings from './settings.vue';
import Applications from './applications.vue';
import Application from './application.vue';
import Countries from './countries.vue';
import Country from './country.vue';
import Languages from './languages.vue';
import Language from './language.vue';
import Users from './users.vue';
import User from './user.vue';
import UserRole from './role.vue';
import Translations from './translations.vue';

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
    component: Settings,
    meta: {
      name: section.name,
      alias: section.alias,
      section: section
    }
  });


  // add details

  addArea(__zero.alias.settings.applications, Applications, Application, true);

  addArea(__zero.alias.settings.countries, Countries, Country, true);

  addArea(__zero.alias.settings.languages, Languages, Language, true);

  addArea(__zero.alias.settings.translations, Translations, Translations, true);

  addArea(__zero.alias.settings.users, Users, User, true, area =>
  {
    routes.push({
      name: 'roles-create',
      path: '/' + section.alias + '/roles/create/:scope?',
      props: true,
      component: UserRole,
      meta: {
        create: true,
        name: area.name
      }
    });

    routes.push({
      name: 'roles-edit',
      path: '/' + section.alias + '/roles/edit/:id',
      props: true,
      component: UserRole,
      meta: {
        name: area.name
      }
    });
  });
}


export default routes;