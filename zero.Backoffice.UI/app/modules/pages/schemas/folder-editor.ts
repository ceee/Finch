
import { ZeroEditor } from '../../../editor/editor';

const editor = new ZeroEditor('pages:zero.folder');

editor.resourcePrefix = '@page.folder.fields.';

editor.field('isPartOfRoute', { optional: true, horizontal: true }).toggle();
editor.field('urlAlias', { optional: true, hidden: x => !x.isPartOfRoute }).text({ maxLength: 40 });

export default editor;