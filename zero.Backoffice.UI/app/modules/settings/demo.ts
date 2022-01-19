
import { ZeroEditor } from "../../editor/editor";
import { formatDate } from '../../utils/dates';

const editor = new ZeroEditor('demo');

editor.field('categoryIds').categoryPicker({ limit: 20, pickChannel: true });
editor.field('video').video();

editor.field('rte', { label: 'Number' }).rte();
editor.field('rte').output({ html: false });
editor.field('link').linkPicker();

editor.field('productId1').productPicker({ variants: true });
//editor.field('productId2').productPicker({ limit: 5 });
//editor.field('productId3').productPicker({ channelDisabled: true, channelId: 'channels.ct639ueyem4q'});

editor.field('name', { label: 'Text' }).text({ maxLength: 120 });
editor.field('isActive', { label: 'Toggle', horizontal: true }).toggle();
editor.field('icon', { label: 'Icon' }).iconPicker();
editor.field('images', { label: 'Images' }).image({ limit: 4 });
editor.field('docs', { label: 'Documents' }).media({ limit: 1 });
editor.field('number', { label: 'Number' }).number();
editor.field('output', { label: 'Number' }).output();
editor.field('textarea', { label: 'Number' }).textarea();
editor.field('state', { label: 'Number' }).state({ items: [{ label: 'Green', value: 'green' }, { label: 'Red', value: 'red' }] });


export default editor;