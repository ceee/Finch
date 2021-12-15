
import Editor from '../../../editor/editor';

const editor = new Editor('countries:edit', '@country.fields.');
editor.blueprintAlias = 'country';

editor.field('name', { label: '@ui.name' }).text(120).required();
//editor.field('alias', { label: '@ui.alias' }).text().required();
editor.field('code').text(2).required();
editor.field('isPreferred').toggle();

export default editor;