import Vue from 'vue';
import VueRouter from 'vue-router';
import Flow from 'view/flow';
import Transaction from 'view/transaction';
import Invoices from 'view/invoices';
import InvoiceEdit from 'view/invoiceedit';
import Clients from 'view/clients';
import Client from 'view/client';
import Changelog from 'view/changelog';
import Rules from 'view/rules';
import Reports from 'view/reports';
import Report from 'view/report';
import ReportCreate from 'view/reportcreate';
import EventHub from 'utils/eventhub.js';

Vue.use(VueRouter);

history.scrollRestoration = 'manual';

const routes = [

  // flow
  { path: '/', component: Flow, name: 'flow', meta: { title: 'Flow' } },
  { path: '/provider/:provider', component: Flow, name: 'flowProvider', props: true, meta: { title: 'Flow' } },
  { path: '/provider/:provider/page/:page', component: Flow, name: 'flowProviderAndPage', props: true, meta: { title: 'Flow' } },
  { path: '/page/:page', component: Flow, name: 'flowPage', props: true, meta: { title: 'Flow' } },
  { path: '/transaction/edit/:id', component: Transaction, name: 'transaction', props: true, meta: { title: 'Transaction' } },

  // invoices
  { path: '/invoices', component: Invoices, name: 'invoices', meta: { title: 'Invoices' } },
  { path: '/invoices/create', component: InvoiceEdit, name: 'invoiceCreate', meta: { title: 'Create invoice' } },
  { path: '/invoices/create/:id', component: InvoiceEdit, name: 'invoiceCreateFrom', props: true, meta: { title: 'Create invoice' } },
  { path: '/invoices/edit/:id', component: InvoiceEdit, name: 'invoiceEdit', props: true, meta: { title: 'Edit invoice' } },
  { path: '/invoices/:year', component: Invoices, name: 'invoicesYear', props: true, meta: { title: 'Invoices' } },

  // clients
  { path: '/clients', component: Clients, name: 'clients', meta: { title: 'Clients' } },
  { path: '/clients/edit/:id', component: Client, name: 'client', props: true, meta: { title: 'Client' } },

  // changelog
  { path: '/changelog', component: Changelog, name: 'changelog', meta: { title: 'Changelog' } },

  // rules
  { path: '/rules', component: Rules, name: 'rules', props: true, meta: { title: 'Rules' } },
  { path: '/rules/items', component: Rules, name: 'rulesItems', meta: { title: 'Rules' } },
  { path: '/rules/history', component: Rules, name: 'rulesHistory', meta: { title: 'Rules' } },
  { path: '/rules/edit/:id?', component: Rules, name: 'rules', props: true, meta: { title: 'Rules' } },

  // reports
  { path: '/reports', component: Reports, name: 'reports', meta: { title: 'Reports' } },
  { path: '/reports/edit/:id', component: Report, name: 'report', props: true, meta: { title: 'Report' } },
  { path: '/reports/create/:type?', component: ReportCreate, name: 'reportCreate', props: true, meta: { title: 'Report' } },
  { path: '/reports/:year', component: Reports, name: 'reportsYear', props: true, meta: { title: 'Reports' } }
];

const router = new VueRouter({
  mode: 'history',
  routes: routes,
  scrollBehavior(to, from, savedPosition)
  {
    return savedPosition ? savedPosition : { x: 0, y: 0 };
    //return new Promise((resolve, reject) =>
    //{
    //  EventHub.$once('page-loaded', () =>
    //  {
    //    console.info('pos', savedPosition);
    //    resolve(savedPosition ? savedPosition : { x: 0, y: 0 });
    //  });
    //});
  }
});

router.beforeEach((to, from, next) =>
{
  document.title = to.meta.title !== null ? to.meta.title + " | fifty" : "fifty";
  EventHub.$emit('dialog-close');
  next();
});

export default router;