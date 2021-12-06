
export const YOUTUBE_REGEX = /youtu(?:\.be|be\.com)\/(?:.*v(?:\/|=)|(?:.*\/)?)([a-zA-Z0-9-_]+)/gim;
export const VIMEO_REGEX = /vimeo\.com\/(\d+)($|\/)/gim;

let youTubeApiKey = "xxx"; // TODO vue use from config

/**
 * Get video ID from an URL.
 * This currently works for YouTube and Vimeo links.
 * @param {string} url
 */
export function getVideoId(url: string): string
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


export interface VideoMetadata
{
  id: string,
  url: string,
  success: boolean,
  data: object,
  image: string,
  title: string,
  description: string
}


/**
 * Get metadata for a vimeo ID.
 * @param {string} id
 */
export async function getVimeoMetadata(id: string): Promise<VideoMetadata>
{
  let result = {
    id,
    url: `https://vimeo.com/${id}`,
    success: false,
    data: null,
    image: null,
    title: null,
    description: null
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
export async function getYoutubeMetadata(id: string): Promise<VideoMetadata>
{
  let result = {
    id,
    image: `https://i3.ytimg.com/vi/${id}/maxresdefault.jpg`,
    url: `https://www.youtube.com/watch?v=${id}`,
    success: false,
    data: null,
    title: null,
    description: null
  };


  if (youTubeApiKey)
  {
    result.success = false;

    let response = await fetch(`https://www.googleapis.com/youtube/v3/videos?part=snippet&id=${id}&key=${youTubeApiKey}`);

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
  }
  else
  {
    result.success = true;
  }

  return result;
};