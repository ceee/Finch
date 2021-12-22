
import { ZeroEditor } from '../../../editor/editor';

const editor = new ZeroEditor('integrations');

editor.resourcePrefix = '@translation.fields.';

const set = editor.fieldset();

set.field('key').text({ maxLength: 300 });
set.field('display').state({
  items: [
    { label: '@translation.display.text', value: 0 },
    { label: '@translation.display.html', value: 1 }
  ]
});

editor.field('value').setHidden(x => x.display === 0).rte();
editor.field('value').setHidden(x => x.display === 1).textarea();

export default editor;