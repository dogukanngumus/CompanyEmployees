﻿namespace Entities;

public class MaxAgeRangeBadRequestException() : BadRequestException("Max age can't be less than min age.")
{

}
