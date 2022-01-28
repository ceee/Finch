
import { ZeroEditor } from '../../../editor/editor';

const editor = new ZeroEditor('translations');

editor.resourcePrefix = '@translation.fields.';


editor.field('key').text({ maxLength: 300 });
editor.field('display', { readonly: x => !x || x.id }).state({
  items: [
    { label: '@translation.display.text', value: 'text' },
    { label: '@translation.display.html', value: 'html' }
  ]
});

editor.field('value', { hidden: x => !x || x.display === 'text' }).rte();
editor.field('value', { hidden: x => !x || x.display === 'html' }).textarea();

export default editor;