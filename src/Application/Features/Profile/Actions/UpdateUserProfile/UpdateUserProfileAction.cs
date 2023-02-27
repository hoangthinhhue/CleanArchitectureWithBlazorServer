﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorState;
using Mgr.Core.Models;
using UnitMgr.Domain.AggregatesModel.IdentityDTOs;

namespace UnitMgr.Application.Features.Identity.Profile;

public partial class UserProfileState
{
    public class UpdateUserProfileAction : IAction
    {
        public UpdateUserProfileAction(UserProfile userProfile)
        {
            UserProfile = userProfile;
        }

        public UserProfile UserProfile { get; private set; }
    }
    public class UpdateUserDtoAction : IAction
    {
        public UpdateUserDtoAction(UserDto userDto)
        {
            UserDto = userDto;
        }
        public UserDto UserDto { get; private set; }
    }
}
