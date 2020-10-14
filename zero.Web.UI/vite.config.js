const aliases = Object.entries({
  '@zero/': '/',
  '@zerosetup/': '/setup/',
  '@shop/': '/../zero.Commerce/Plugins/zero.Commerce/'
});

export default {

  outDir: 'Assets/',

  base: 'zero-cli/',

  resolvers: [{
    alias(path)
    {
      for (const [alias, resolved] of aliases) 
      {
        if (path.startsWith(alias)) 
        {
          return path.replace(alias, resolved);
        }
      }
    },
    //resolveRelativeRequest(publicPath, relativePublicPath)
    //{
    //  console.info(publicPath + " | " + relativePublicPath);
    //}
  }],

  optimizeDeps: {
    include: ['qs'],
  },
};