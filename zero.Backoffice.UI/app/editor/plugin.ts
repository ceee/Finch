import { ZeroPlugin, ZeroPluginOptions } from '../core';
import { ZeroEditor } from './editor';
import createFields from './fields/createFields';

const editor = new ZeroEditor('integration.analytics.fathom');
editor.field('siteId').text({ maxLength: 30 });
editor.field('customDomain', { optional: true }).text({ maxLength: 150 });

const editor2 = new ZeroEditor('integration.analytics.google');
editor2.field('trackingId').text({ maxLength: 30 });
editor2.field('siteVerificationId').text({ maxLength: 150 });

export default {
  name: "zero.editor",

  install(app: ZeroPluginOptions)
  {
    createFields(app);

    app.schema('integration.analytics.fathom', editor);
    app.schema('integration.analytics.google', editor2);
  }
} as ZeroPlugin;