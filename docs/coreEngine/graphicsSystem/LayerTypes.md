# 2.2 - Layer Types
Kasane2D already comes with a selection of different layer types available.

## Tilemap Surface
A tilemap surface is used to, as the name suggests, render a tilemap to the screen. For each tilemap surface, the resolution of the individual tiles in pixels, and the dimensions of the surface in tiles are freely configurable. Neither of those values require a perfect square resolution.

A single tilemap shares an `ISpriteAtlas` that is used for the entire tilemap. To change what is rendered for a specific tile, you just update the atlas coordinates of the respective tile. In addition, each tile's graphics can be horizontally and/or vertically flipped, as well.

## Texture Surface
A texture surface consists of, as the name suggests, a single texture. Just like tilemap surfaces, texture surfaces have a scrollable viewport. Texture surfaces are used as a target for [free-form rendering].

## Sprite Layer
Sprite layers are the place where sprite graphics not bound to a tile grid are living. For the retro nerds: these layers are Kasane2D's modern equivalent of the OAM in NES's and SNES's PPU. Unlike with old retro hardware, the number of sprites in a sprite layer and the number of sprite layers available can be freely configured. There also is not a limit to how many sprites can be rendered in one scan line. If a sprite layer has 64 sprites, all of those 64 sprites can be rendered on the same line without a single one of them being dropped.

Each sprite has its own `ISpriteAtlas` that determines its graphics. Just like tiles, you just update the sprite's atlas index to change the graphics for this sprite. The graphics can be horizontally and/or vertically flipped, too.
Unlike tiles, a sprite has a position property that determines its position on the screen. A position of (0, 0) means that the top-left pixel of the sprite is rendered at the top-left pixel of the viewport.
Sprites also have an `IsActive` property. Deactivated sprites are not rendered to the screen.

Unlike other surface types, sprite layers can't be scrolled. Instead, the internal surface of a sprite layer has a fixed size that extends outside the screen boundaries by the respective size of the layer's sprites. This is done so that a sprite can be smoothly scrolled inside and outside of the screen. All sprites who's position is outside of this surface are ignored for rendering.
An example for illustration:
Let's say we have a viewport the size of 320x240 pixels and a sprite layer with 16x16 pixels sprites. This means that the surface of this sprite layer extends from (-16, -16) all the way to (335, 255). Only the area from (0, 0) to (319, 239) gets rendered to the screen. Sprites outside the surface area are ignored for rendering.