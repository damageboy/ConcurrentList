// <copyright file="PexAssemblyInfo.cs" company="Microsoft">Copyright © Microsoft 2011</copyright>
using Microsoft.Pex.Framework.Creatable;
using Microsoft.Pex.Framework.Instrumentation;
using Microsoft.Pex.Framework.Moles;
using Microsoft.Pex.Framework.Settings;
using Microsoft.Pex.Framework.Validation;
using Microsoft.Pex.Framework.Suppression;
using NUnit.Framework;
using ConcurrentList.Threading;
using System.IO;
using System;

// Microsoft.Pex.Framework.Settings
[assembly: PexAssemblySettings(TestFramework = "NUnit")]

// Microsoft.Pex.Framework.Instrumentation
[assembly: PexAssemblyUnderTest("ConcurrentList")]

// Microsoft.Pex.Framework.Creatable
[assembly: PexCreatableFactoryForDelegates]

// Microsoft.Pex.Framework.Validation
[assembly: PexAllowedContractRequiresFailureAtTypeUnderTestSurface]
[assembly: PexAllowedXmlDocumentedException]

// Microsoft.Pex.Framework.Moles
[assembly: PexAssumeContractEnsuresFailureAtBehavedSurface]
[assembly: PexChooseAsBehavedCurrentBehavior]

[assembly: PexInstrumentAssembly("ConcurrentList")]
[assembly: PexSuppressStaticFieldStore(typeof(Assert), "counter")]
[assembly: PexSuppressUninstrumentedMethodFromType("Microsoft.Win32.SafeNativeMethods")]
[assembly: PexSuppressUninstrumentedMethodFromType(typeof(CrazyParallel))]
[assembly: PexSuppressUninstrumentedMethodFromType(typeof(TextWriter))]
[assembly: PexSuppressUninstrumentedMethodFromType("ConcurrentList.Threading.ThreadJoiner")]
[assembly: PexSuppressUninstrumentedMethodFromType(typeof(Random))]
[assembly: PexSuppressUninstrumentedMethodFromType(typeof(Environment))]
[assembly: PexSuppressUninstrumentedMethodFromType(typeof(SaneParallel))]
[assembly: PexSuppressUninstrumentedMethodFromType("ConcurrentList.Threading.TaskJoiner")]
