
import { ZeroEditor } from "../../../editor/editor";

const editor = new ZeroEditor();

editor.resourcePrefix = '@mailTemplate.fields.';

const general = editor.tab('general', '@ui.tab_general');
const sender = editor.tab('sender', '@mailTemplate.tabs.sender');
const recipient = editor.tab('recipient', '@mailTemplate.tabs.recipient');

general.field('name', { label: '@ui.name' }).text({ maxLength: 60 });
general.field('key').text({ maxLength: 60 });
general.field('subject').text({ maxLength: 80 });
general.field('body', { optional: true }).rte();
general.field('preheader', { optional: true }).text({ maxLength: 160 });
sender.field('senderEmail', { optional: true }).text();
sender.field('senderName', { optional: true }).text();
recipient.field('recipientEmail', { optional: true }).text();
recipient.field('cc', { optional: true }).text();
recipient.field('bcc', { optional: true }).text();

export default editor;