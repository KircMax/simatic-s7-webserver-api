﻿// Copyright (c) 2023, Siemens AG
//
// SPDX-License-Identifier: MIT
using System.Collections.Generic;

namespace Siemens.Simatic.S7.Webserver.API.Models.Responses
{
    /// <summary>
    /// ApiPlcProgramBrowseCodeBlocksResponse (Jsonrpc,id) with a List of ApiPlcProgramBrowseCodeBlocksData
    /// </summary>
    public class ApiPlcProgramBrowseCodeBlocksResponse : ApiResultResponse<List<ApiPlcProgramBrowseCodeBlocksData>>
    {
    }
}
