
import { ZeroEditor } from "../../../schemas/editor/editor";

const editor = new ZeroEditor();

editor.field('name').text({ maxLength: 120 });
editor.field('code').text({ maxLength: 2 });
editor.field('isPreferred', { optional: true }).toggle();

export default editor;