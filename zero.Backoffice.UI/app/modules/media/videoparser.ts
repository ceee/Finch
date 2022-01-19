
export const YOUTUBE_REGEX = /youtu(?:\.be|be\.com)\/(?:.*v(?:\/|=)|(?:.*\/)?)([a-zA-Z0-9-_]+)/gim;
export const VIMEO_REGEX = /vimeo\.com\/(\d+)($|\/)/gim;


/**
 * Get video ID from an URL.
 * This currently works for YouTube and Vimeo links.
 * @param {string} url
 */
export let getVideoId = url =>
{
  if (!url)
  {
    return null;
  }

  if (url.indexOf('vimeo.com') > -1)
  {
    let matches = VIMEO_REGEX.exec(url);
    return matches && matches[1];
  }
  else if (url.indexOf('youtu') > -1)
  {
    let matches = YOUTUBE_REGEX.exec(url);
    return matches && matches[1];
  }

  return null;
};


/**
 * Get metadata for a vimeo ID.
 * @param {string} id
 */
export let getVimeoMetadata = async id =>
{
  let result = {
    id,
    url: `https://vimeo.com/${id}`,
    success: false
  };

  let response = await fetch(`https://vimeo.com/api/v2/video/${id}.json`);

  if (response.ok)
  {
    let data = await response.json();
    result.data = data[0];
    result.image = result.data.thumbnail_large;
    result.title = result.data.title;
    result.description = result.data.description;
    result.success = true;
  }

  return result;
};


/**
 * Get metdata for a YouTube ID.
 * @param {string} url
 */
export let getYoutubeMetadata = async id =>
{
  const apiKey = 'AIzaSyD720TVsYEil4PTESJ9jD0Xijd0zldExmc'; // TODO v3 make configurable

  let result = {
    id,
    image: `https://i3.ytimg.com/vi/${id}/maxresdefault.jpg`,
    url: `https://www.youtube.com/watch?v=${id}`,
    success: true
  };

  if (apiKey)
  {
    result.success = false;

    let response = await fetch(`https://www.googleapis.com/youtube/v3/videos?part=snippet&id=${id}&key=${apiKey}`);

    if (response.ok)
    {
      let data = await response.json();

      if (data && data.items && data.items.length)
      {
        result.data = data.items[0].snippet;

        let thumbs = Object.values(result.data.thumbnails);
        let thumb = thumbs[thumbs.length - 1];
        result.image = thumb.url;
        result.title = result.data.title;
        result.description = result.data.description;
        result.success = true;
      }
    }

    return result;
  }
  else
  {
    return {
      id,
      image: `https://i3.ytimg.com/vi/${id}/maxresdefault.jpg`,
      url: `https://www.youtube.com/watch?v=${id}`,
      success: true
    };
  }
};