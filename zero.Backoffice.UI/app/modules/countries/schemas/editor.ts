
import { ZeroEditor } from "../../../editor/editor";
import { formatDate } from '../../../utils/dates';

const editor = new ZeroEditor('countries');

editor.resourcePrefix = '@country.fields.';

editor.field('name').text({ maxLength: 120 });
editor.field('code').text({ maxLength: 2 });
editor.field('isPreferred', { optional: true, horizontal: true }).toggle();

export default editor;