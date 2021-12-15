
import Editor from '../../../editor/editor';

const editor = new Editor('languages:edit', '@language.fields.');
editor.blueprintAlias = 'language';
editor.field('name', { label: '@ui.name' }).text(60).required();
editor.field('code').text(10).required();
editor.field('inheritedLanguageId').languagePicker();
editor.field('isDefault').toggle();
editor.field('isOptional').toggle();

export default editor;