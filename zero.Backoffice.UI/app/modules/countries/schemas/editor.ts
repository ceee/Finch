
import { Editor } from '../../../editor/new/editor';

const editor = new Editor(); //'@country.fields.');

const field = editor.field('name').setRequired(true).text({ maxLength: 80 });

console.info(editor);

export default editor;

//import Editor from '../../../editor/editor';

//const editor = new Editor('@country.fields.');
//editor.blueprintAlias = 'countries';

//editor.field('name', { label: '@ui.name' }).text(120).required();
////editor.field('alias', { label: '@ui.alias' }).text().required();
//editor.field('code').text(2).required();
//editor.field('isPreferred').toggle();

//export default editor;