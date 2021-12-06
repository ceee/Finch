

/**
 * Converts html to text
 * @returns {string} Text
 */
export function convertHtmlToText(html: string, preserveNewLines: boolean = false): string
{
  if (preserveNewLines)
  {
    html = html
      .replaceAll('<br>', '___nl___')
      .replaceAll('<br >', '___nl___')
      .replaceAll('<br/>', '___nl___')
      .replaceAll('<br />', '___nl___')
      .replaceAll('<li>', '___nl___')
      .replaceAll('<p>', '___nl___');
  }
  let tag = document.createElement('div');
  tag.innerHTML = html;
  return tag.innerText.replaceAll('___nl___', '<br>');
}