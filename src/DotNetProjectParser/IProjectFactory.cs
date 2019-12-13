// -----------------------------------------------------------------------
//  <copyright file="IProjectFactory.cs" company="SDL plc">
//   Copyright (c) SDL plc. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using System.IO;

namespace DotNetProjectParser
{
    public interface IProjectFactory
    {
        Project GetProject(FileInfo projectFile);
    }
}