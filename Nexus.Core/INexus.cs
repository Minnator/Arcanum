﻿namespace Nexus.Core;

public interface INexus
{
   object _getValue(Enum property);

   void _setValue(
      Enum property, object value);
}