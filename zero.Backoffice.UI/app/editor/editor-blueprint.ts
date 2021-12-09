
import { localize } from '../services/localization';
import { arrayRemove } from '../utils/arrays';

function unlocked(config, model, field)
{
  if (!config || !model || !model.blueprint)
  {
    return true;
  }

  return canUnlock(config, model, field)
      && (model.blueprint.desync.indexOf(field.path) > -1 
      || model.blueprint.desync.find(x => field.path.indexOf(x + '.') === 0) != null);
}


function canUnlock(config, model, field)
{
  if (!config || !model || !model.blueprint)
  {
    return false;
  }

  return config.unlocked.indexOf(field.path) > -1 
      || config.unlocked.find(x => field.path.indexOf(x + '.') === 0) != null;
}


function isBlueprintChild(config, route, model)
{
  return model && route.query.scope != 'shared' && !!model.blueprint;
}


function isBlueprintParent(config, route, model)
{
  return model && route.query.scope == 'shared' && !model.blueprint;
}


async function lock(config, model, field)
{
  arrayRemove(model.blueprint.desync, field.path);
  //let blueprint = await BlueprintApi.getById(model.blueprint.id);
  //return blueprint[field.path];
}


function unlock(config, model, field)
{
  model.blueprint.desync.push(field.path);
}


/**
 * [TODO] description
 * @typedef {object} EditorBlueprint
 * @param {string} alias - Alias which is used to match an editor
 * @param {function} unlocked - Whether a field is unlocked and can be changed
 * @param {function} canUnlock - Whether a field can be unlocked
 * @param {EditorBlueprintField[]} fields - Fields which can be altered in synchronization
 */

/**
 * [TODO] description
 * @typedef {object} EditorBlueprintField
 * @param {string} path - Property path
 * @param {string} label - Label which describes the property
 * @param {string} description - Additional description for this property
 * @param {function} synced - Whether this field is synchronized with the parent
 */

/**
 * [TODO] description
 * @param {string} alias - Alias for the tab
 * @returns {EditorBlueprint}
 */
export function createBlueprintConfig(zero, editor, model)
{
  let blueprintAlias = editor.blueprintAlias;
  let config = zero.config.blueprints.find(x => x.alias == blueprintAlias);

  if (!blueprintAlias || !config)
  {
    return {
      alias: blueprintAlias,
      unlocked: () => true,
      canUnlock: () => false,
      isBlueprintParent: () => false,
      isBlueprintChild: () => false,
      lock: () => {},
      unlock: () => {},
      fields: []
    };
  }

  

  // check for blueprint availability
  //config.isParent = this.value && this.$route.query.scope === 'shared';
  //config.isNewParent = config.isParent && !this.value.id;
  //config.isChild = !config.isParent && this.value && !!this.value.blueprint;

  // get all editor fields
  let fields = [];
  let processed = ['blueprint'];
  let defaults = ['name', /*'alias',*/ 'isActive', 'sort', /*'key'*/];

  // add blueprint setting for default fields
  defaults.forEach(alias =>
  {
    processed.push(alias);

    if (config.unlocked.indexOf(alias) > -1)
    {
      fields.push({
        path: alias,
        label: localize("@ui.entityfields." + alias),
        description: localize("@ui.entityfields." + alias + "_text", { hideEmpty: true }),
        synced: model => !model.blueprint || model.blueprint.desync.indexOf(alias) < 0
      });
    }
  });

  // add blueprint setting for custom fields
  editor.fields.forEach(field =>
  {
    let alias = field.path;

    if (processed.indexOf(alias) < 0)
    {
      processed.push(alias);

      if (config.unlocked.indexOf(alias) > -1 || config.unlocked.find(x => alias.indexOf(x + '.') === 0))
      {
        fields.push({
          path: alias,
          label: field.options.label || editor.templateLabel(alias),
          description: localize(field.options.description || editor.templateDescription(alias), { hideEmpty: true }),
          synced: model => !model.blueprint || model.blueprint.desync.indexOf(alias) < 0
        });
      }
    }
  });

  return {
    alias: blueprintAlias,
    unlocked: (model, field) => unlocked(config, model, field),
    canUnlock: (model, field) => canUnlock(config, model, field),
    unlock: async (model, field) => await unlock(config, model, field),
    lock: async (model, field) => await lock(config, model, field),
    isBlueprintParent: (route, model) => isBlueprintParent(config, route, model),
    isBlueprintChild: (route, model) => isBlueprintChild(config, route, model),
    fields: fields
  };
};