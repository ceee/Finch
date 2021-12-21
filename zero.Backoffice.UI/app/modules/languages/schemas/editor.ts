
import { ZeroEditor } from '../../../editor/editor';

const editor = new ZeroEditor();

editor.resourcePrefix = '@language.fields.';

//editor.blueprintAlias = 'languages';
editor.field('name', { label: '@ui.name' }).text({ maxLength: 60 });
editor.field('code').text({ maxLength: 5 });
editor.field('inheritedLanguageId', { optional: true }).languagePicker();
editor.field('isDefault', { optional: true, horizontal: true }).toggle();
editor.field('isOptional', { optional: true, horizontal: true }).toggle();

export default editor;