import { ZeroPlugin, ZeroPluginOptions } from '../core';
import createFields from './fields/createFields';

export default {
  name: "zero.editor",

  install(app: ZeroPluginOptions)
  {
    createFields(app);
  }
} as ZeroPlugin;