﻿// Copyright (c) 2021, Siemens AG
//
// SPDX-License-Identifier: MIT
using Siemens.Simatic.S7.Webserver.API.Enums;
using Siemens.Simatic.S7.Webserver.API.Exceptions;
using Siemens.Simatic.S7.Webserver.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Siemens.Simatic.S7.Webserver.API.StaticHelpers
{
    /// <summary>
    /// Static class to check Request Parameters
    /// </summary>
    public static class RequestParameterChecker
    {
        /// <summary>
        /// Check ApiWebAppState => None isnt valid!
        /// </summary>
        /// <param name="apiWebAppState">Web Application State</param>
        /// <param name="performCheck">Bool to determine wether to really perform the check or not</param>
        public static void CheckState(ApiWebAppState apiWebAppState, bool performCheck)
        {
            if(performCheck)
            {
                if (apiWebAppState == ApiWebAppState.None)
                {
                    throw new ApiInvalidParametersException($"WebApp function shall not be called with state None:{ Environment.NewLine + apiWebAppState.ToString() }" +
                    $"{Environment.NewLine}Probably Api would send: ", new ApiException(new Responses.ApiErrorModel() { Error = new Models.ApiError() { Code = ApiErrorCode.InvalidParams, Message = "Invalid Params" } }));
                }
            }
        }

        /// <summary>
        /// valid charset: "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.+\""
        /// </summary>
        /// <param name="webAppName">Name of the Web Application</param>
        /// <param name="performCheck">Bool to determine wether to really perform the check or not</param>
        public static void CheckWebAppName(string webAppName, bool performCheck)
        {
            if (performCheck)
            {
                if (webAppName.Length == 0)
                {
                    throw new ApiInvalidParametersException($"the webapp name cannot be an empty string! :{Environment.NewLine + webAppName + Environment.NewLine}is not valid!",
                        new ApiException(new Responses.ApiErrorModel() { Error = new Models.ApiError() { Code = ApiErrorCode.InvalidParams, Message = "Invalid Params" } }));
                }
                if (webAppName.Length > 100)
                {
                    throw new ApiInvalidApplicationNameException($"the max. allowed length for a webapp is 100 chars! - therefor :{Environment.NewLine + webAppName + Environment.NewLine}is not valid!",
                        new ApiException(new Responses.ApiErrorModel() { Error = new Models.ApiError() { Code = ApiErrorCode.InvalidApplicationName, Message = "Invalid application name" } }));
                }
                string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.+\"";
                if (!webAppName.All(c => validChars.Contains(c)))
                {
                    throw new ApiInvalidApplicationNameException($"Invalid characters found in:{Environment.NewLine + webAppName + Environment.NewLine} correct chars:{Environment.NewLine + validChars}",
                         new ApiException(new Responses.ApiErrorModel() { Error = new Models.ApiError() { Code = ApiErrorCode.InvalidApplicationName, Message = "Invalid application name" } }));
                }
            }
        }

        /// <summary>
        /// None isnt valid (unsupported type = -1)
        /// </summary>
        /// <param name="apiPlcProgramData">PlcProgramData</param>
        /// <param name="performCheck">Bool to determine wether to really perform the check or not</param>
        public static void CheckPlcProgramWriteOrReadDataType(ApiPlcProgramDataType apiPlcProgramData, bool performCheck)
        {
            if (performCheck)
            {
                if (apiPlcProgramData == ApiPlcProgramDataType.None)
                {
                    throw new ApiHelperInvalidPlcProgramDataTypeException($"PlcProgram Read or Write Comfort functions are not available without the DataType!(Given:{apiPlcProgramData.ToString()})" +
                        $"{Environment.NewLine}Browse for the PlcProgramData first, set it and then use comfort functionality with given DataType!");
                }
                var bytesOfDataType = apiPlcProgramData.GetBytesOfDataType();
                // unsupported: -1
                if (bytesOfDataType == -1)
                {
                    throw new ApiUnsupportedAddressException(new ApiException(new Responses.ApiErrorModel() { Error = new ApiError() { Code = ApiErrorCode.UnsupportedAddress, Message = "Unsupported Address" } }));
                }
            }
        }

        /// <summary>
        /// valid charset: "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.+()|,/.*!\"'"
        /// </summary>
        /// <param name="resourceName">Name of the resource that should be checked for the valid charset</param>
        /// <param name="performCheck">Bool to determine wether to really perform the check or not</param>
        public static void CheckResourceName(string resourceName, bool performCheck)
        {
            if (performCheck)
            {
                if (resourceName.Length == 0)
                {
                    throw new ApiInvalidParametersException($"the resource name cannot be an empty string! :{Environment.NewLine + resourceName + Environment.NewLine}is not valid!",
                        new ApiException(new Responses.ApiErrorModel() { Error = new Models.ApiError() { Code = ApiErrorCode.InvalidParams, Message = "Invalid Params" } }));
                }
                if (resourceName.Length > 200)
                {
                    throw new ApiInvalidResourceNameException($"the max. allowed length for a resource is 200 chars! - therefor :{Environment.NewLine + resourceName + Environment.NewLine}is not valid!",
                        new ApiException(new Responses.ApiErrorModel() { Error = new Models.ApiError() { Code = ApiErrorCode.InvalidResourceName, Message = "Invalid resource name" } }));
                }
                string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.+()|,/.*!\"'";
                if (!resourceName.All(c => validChars.Contains(c)))
                {
                    throw new ApiInvalidResourceNameException($"Invalid characters found in:{Environment.NewLine + resourceName + Environment.NewLine} correct chars:{Environment.NewLine + validChars}",
                         new ApiException(new Responses.ApiErrorModel() { Error = new Models.ApiError() { Code = ApiErrorCode.InvalidResourceName, Message = "Invalid resource name" } }));
                }
            }
        }

        /// <summary>
        /// Only run or stop are valid!
        /// </summary>
        /// <param name="plcOperatingMode">Operating mode that should be checked for being valid</param>
        /// <param name="performCheck">Bool to determine wether to really perform the check or not</param>
        public static void CheckPlcRequestChangeOperatingMode(ApiPlcOperatingMode plcOperatingMode, bool performCheck)
        {
            if (performCheck)
            {
                if (plcOperatingMode == ApiPlcOperatingMode.Run || plcOperatingMode == ApiPlcOperatingMode.Stop)
                    return;
                throw new ApiInvalidParametersException($"Plc.RequestChangeOperatingMode shall not be called with { Environment.NewLine + plcOperatingMode.ToString().ToLower() }" +
                    $"{Environment.NewLine}Probably Api would send: ", new ApiException(new Responses.ApiErrorModel() { Error = new Models.ApiError() { Code = ApiErrorCode.InvalidParams, Message = "Invalid Params" } }));
            }
        }

        /// <summary>
        /// None isnt valid!
        /// </summary>
        /// <param name="plcProgramBrowseMode">PlcProgramBrowseMode that should be checked for being valid</param>
        /// <param name="performCheck">Bool to determine wether to really perform the check or not</param>
        public static void CheckPlcProgramBrowseMode(ApiPlcProgramBrowseMode plcProgramBrowseMode, bool performCheck)
        {
            if (performCheck)
            {
                if (plcProgramBrowseMode == ApiPlcProgramBrowseMode.None)
                {
                    throw new ApiInvalidParametersException($"PlcProgram.Browse shall not be called with { Environment.NewLine + plcProgramBrowseMode.ToString().ToLower() }" +
                    $"{Environment.NewLine}Probably Api would send: ", new ApiException(new Responses.ApiErrorModel() { Error = new Models.ApiError() { Code = ApiErrorCode.InvalidParams, Message = "Invalid Params" } }));
                }
            }
        }

        /// <summary>
        /// None isnt valid!
        /// </summary>
        /// <param name="apiPlcProgramReadMode">PlcProgramReadMode  that should be checked for being valid</param>
        /// <param name="performCheck">Bool to determine wether to really perform the check or not</param>
        public static void CheckPlcProgramReadOrWriteMode(ApiPlcProgramReadOrWriteMode? apiPlcProgramReadMode, bool performCheck)
        {
            if (performCheck)
            {
                if (apiPlcProgramReadMode != null)
                {
                    if (apiPlcProgramReadMode == ApiPlcProgramReadOrWriteMode.None)
                    {
                        throw new ApiInvalidParametersException($"PlcProgram.Read or Write shall not be called with { Environment.NewLine + apiPlcProgramReadMode.ToString().ToLower() }" +
                        $"{Environment.NewLine}Probably Api would send: ", new ApiException(new Responses.ApiErrorModel() { Error = new Models.ApiError() { Code = ApiErrorCode.InvalidParams, Message = "Invalid Params" } }));
                    }
                }
            }
        }

        /// <summary>
        /// 28 Chars is the only accepted length for the ticketId!
        /// </summary>
        /// <param name="ticketId">TicketId that should be checked for being valid</param>
        /// <param name="performCheck">Bool to determine wether to really perform the check or not</param>
        public static void CheckTicket(string ticketId, bool performCheck)
        {
            if (performCheck)
            {
                if (ticketId.Length != 28)
                {
                    throw new ApiInvalidParametersException($"Api Tickets cannot have a length other than 28 bytes!{ Environment.NewLine + ticketId + Environment.NewLine }provide a valid ticket!" +
                                        $"{Environment.NewLine}Probably Api would send: ", new ApiException(new Responses.ApiErrorModel() { Error = new Models.ApiError() { Code = ApiErrorCode.InvalidParams, Message = "Invalid Params" } }));
                }
            }
        }

        /// <summary>
        /// an etag has got a max. amount of characters of 128
        /// </summary>
        /// <param name="etag">etag (of resource) to be checked</param>
        /// <param name="performCheck">Bool to determine wether to really perform the check or not</param>
        public static void CheckETag(string etag, bool performCheck)
        {
            if (performCheck)
            {
                if (etag.Length > 128)
                {
                    throw new ApiInvalidETagException($"WebApp.CreateResource shall not be called with \"etag\" { Environment.NewLine + etag } because the value is too long!-max 128 bytes(chars)" +
                        $"{Environment.NewLine}Probably Api would send: ", new ApiException(new Responses.ApiErrorModel() { Error = new Models.ApiError() { Code = ApiErrorCode.InvalidParams, Message = "Invalid Params" } }));
                }
            }
        }

        /// <summary>
        /// Not checking anything currently!
        /// </summary>
        /// <param name="mediaType">MediaType that should be checked for being valid</param>
        /// <param name="performCheck">Bool to determine wether to really perform the check or not</param>
        public static void CheckMediaType(string mediaType, bool performCheck)
        {
            if (performCheck)
            {
                ;
            }
                // could provide a insanely long list of possible mediaTypes look: https://www.iana.org/assignments/media-types/media-types.xhtml - will not do it until requested!
        }

        /// <summary>
        /// None isnt valid!
        /// </summary>
        /// <param name="apiWebAppResourceVisibility">ResourceVisibility that should be checked for being valid</param>
        /// <param name="performCheck">Bool to determine wether to really perform the check or not</param>
        public static void CheckVisibility(ApiWebAppResourceVisibility apiWebAppResourceVisibility, bool performCheck)
        {
            if (performCheck)
            {
                if (apiWebAppResourceVisibility == ApiWebAppResourceVisibility.None)
                    throw new ApiInvalidParametersException($"WebApp.CreateResource shall not be called with { Environment.NewLine + apiWebAppResourceVisibility.ToString().ToLower() }" +
                $"{Environment.NewLine}Probably Api would send: ", new ApiException(new Responses.ApiErrorModel() { Error = new Models.ApiError() { Code = ApiErrorCode.InvalidParams, Message = "Invalid Params" } }));
            }
        }

        /// <summary>
        /// regex used: Regex regex = new Regex(@"\d{4}(-\d{2})(-\d{2})T(\d{2}):(\d{2}):(\d{2})(\.[0-9]{1,3})*Z"); string has to match!
        /// </summary>
        /// <param name="lastModified">LastModified that should be checked for being valid</param>
        /// <param name="performCheck">Bool to determine wether to really perform the check or not</param>
        public static void CheckLastModified(string lastModified, bool performCheck)
        {
            if (performCheck)
            {
                //Regex regex = new Regex(@"\d{4}(-\d{2})(-\d{2})T(\d{2}):(\d{2}):(\d{2})(\.[0-9]{1,3})*Z");
                //if (!regex.IsMatch(lastModified))
                //{
                //    throw new ApiInvalidParametersException($"the datetime provided does not match the pattern:{Environment.NewLine + lastModified + Environment.NewLine + DateTimeFormatting.ApiDateTimeFormat + Environment.NewLine}used Regex:{regex.ToString()}"/*, formatException*//*);
                //}
                // for now not 100% sure the regex check is fine => let the plc perform the check for now/wait for a request to check locally
                ;
            }
        }
    }
}